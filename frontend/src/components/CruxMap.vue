<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useCruxStore } from '@/stores/crux';
import 'leaflet/dist/leaflet.css';
import { LMap, LTileLayer, LMarker, LPopup, LIcon, LLayerGroup, LControl } from '@vue-leaflet/vue-leaflet';
import { useRouter } from 'vue-router';

const cruxStore = useCruxStore();
const zoom = ref(13);
const center = ref([59.9139, 10.7522] as [number, number]); // Fixed type by explicitly typing as tuple
const mapReady = ref(false);
const markerIcon = ref<any>(null); // Changed type to any to fix type error
const mapInstance = ref<any>(null); // Reference to the map instance
const router = useRouter();
const isLocating = ref(false);
const activeBaseLayer = ref('openstreetmap');

// Define available base layers
const baseLayers = [
  { 
    id: 'openstreetmap', 
    name: 'OpenStreetMap', 
    url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png',
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
  },
  { 
    id: 'norgeskart-topo', 
    name: 'Norgeskart (Topographic)', 
    url: 'https://cache.kartverket.no/v1/wmts/1.0.0/topo/default/webmercator/{z}/{y}/{x}.png',
    attribution: '&copy; <a href="http://www.kartverket.no/">Kartverket</a>'
  },
  { 
    id: 'norgeskart-gray', 
    name: 'Norgeskart (Gray)', 
    url: 'https://cache.kartverket.no/v1/wmts/1.0.0/topograatone/default/webmercator/{z}/{y}/{x}.png',
    attribution: '&copy; <a href="http://www.kartverket.no/">Kartverket</a>'
  }
];

// Function to change the base layer
const changeBaseLayer = (layerId: string) => {
  activeBaseLayer.value = layerId;
};

// Function to format crux level as stars
const formatLevel = (level?: number) => {
  if (!level) return '';
  const stars = '★'.repeat(level);
  return stars;
};

// Function to get user's current location
const getCurrentLocation = () => {
  if (!navigator.geolocation) {
    alert('Geolocation is not supported by your browser');
    return;
  }
  
  isLocating.value = true;
  
  navigator.geolocation.getCurrentPosition(
    (position) => {
      const { latitude, longitude } = position.coords;
      center.value = [latitude, longitude];
      if (mapInstance.value) {
        mapInstance.value.setView([latitude, longitude], zoom.value);
      }
      isLocating.value = false;
    },
    (error) => {
      console.error('Error getting location:', error);
      alert('Unable to retrieve your location');
      isLocating.value = false;
    }
  );
};

// Fetch crux items when the component is mounted
onMounted(async () => {
  // Fix for Leaflet marker icons in production
  const leaflet = await import('leaflet');
  // @ts-ignore
  delete leaflet.Icon.Default.prototype._getIconUrl;
  leaflet.Icon.Default.mergeOptions({
    iconRetinaUrl: '/marker-icon-2x.png',
    iconUrl: '/marker-icon.png',
    shadowUrl: '/marker-shadow.png',
  });
  
  markerIcon.value = leaflet.icon({
    iconUrl: '/marker-icon.png',
    iconSize: [25, 41],
    iconAnchor: [12, 41],
    popupAnchor: [1, -34],
    shadowUrl: '/marker-shadow.png',
    shadowSize: [41, 41]
  });
  
  // Fetch the crux data
  if (cruxStore.items.length === 0) {
    await cruxStore.fetchItems();
  }
  
  // Center the map on crux items if available
  if (cruxStore.items.length > 0) {
    const firstCrux = cruxStore.items[0];
    center.value = [parseFloat(firstCrux.lat || '59.9139'), parseFloat(firstCrux.lng || '10.7522')];
  }
  
  mapReady.value = true;
});

// Method to open popup when clicking on a marker
const handleMarkerClick = (crux: any) => {
  // navigate to the crux details view
  console.log('Clicked on crux:', crux.name); 
  router.push(`/crux/${crux.id}`);
};

// Store map reference when ready
const onMapReady = (map: any) => {
  mapInstance.value = map;
};
</script>

<template>
    <div class="map-wrapper">
      <l-map
        v-model:zoom="zoom"
        :center="center"
        class="map"
        @ready="onMapReady"
      >
        <!-- Render the active base layer -->
        <template v-for="layer in baseLayers" :key="layer.id">
          <l-tile-layer
            v-if="layer.id === activeBaseLayer"
            :url="layer.url"
            layer-type="base"
            :name="layer.name"
            :attribution="layer.attribution"
          ></l-tile-layer>
        </template>
        
        <!-- Display markers for each crux -->
        <template v-for="crux in cruxStore.items" :key="crux.id">
          <l-marker 
            v-if="crux.lat && crux.lng"
            :lat-lng="[parseFloat(crux.lat), parseFloat(crux.lng)]"
            @click="handleMarkerClick(crux)"
          >
            <l-popup>
              <div class="popup-content">
                <h3>{{ crux.name }}</h3>
                <p v-if="crux.level" class="difficulty">
                  Difficulty: <span class="stars">{{ formatLevel(crux.level) }}</span>
                </p>
                <p v-if="crux.description">{{ crux.description }}</p>
              </div>
            </l-popup>
          </l-marker>
        </template>
      </l-map>
      
      <!-- Layer control -->
      <div class="layer-control">
        <button 
          v-for="layer in baseLayers" 
          :key="layer.id"
          @click="changeBaseLayer(layer.id)"
          :class="{ 'active': activeBaseLayer === layer.id }"
          :title="layer.name"
        >
          {{ layer.name }}
        </button>
      </div>
      
      <!-- Location button -->
      <button 
        class="location-button" 
        @click="getCurrentLocation"
        :class="{ 'locating': isLocating }"
        title="Find my location"
      >
        <span class="location-icon">📍</span>
      </button>
    </div>
</template>

<style scoped>
.map-wrapper {
  width: 100%;
  height: 90vh;
  border-radius: 8px;
  overflow: hidden;
  position: relative;
}

.map {
  width: 100%;
  height: 100%;
}

.layer-control {
  position: absolute;
  top: 10px;
  right: 10px;
  z-index: 1000;
  background: white;
  border-radius: 4px;
  box-shadow: 0 1px 5px rgba(0,0,0,0.4);
  padding: 6px;
  display: flex;
  flex-direction: column;
}

.layer-control button {
  margin: 3px 0;
  padding: 8px 12px;
  background: white;
  border: 1px solid #ccc;
  border-radius: 4px;
  cursor: pointer;
  font-size: 12px;
  text-align: center;
  transition: all 0.2s ease;
}

.layer-control button:hover {
  background-color: #f0f0f0;
}

.layer-control button.active {
  background-color: #4CAF50;
  color: white;
  border-color: #4CAF50;
}

.location-button {
  position: absolute;
  bottom: 20px;
  right: 20px;
  z-index: 1000;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: white;
  border: none;
  box-shadow: 0 1px 5px rgba(0,0,0,0.4);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.location-button:hover {
  background-color: #f0f0f0;
}

.location-button:active {
  transform: scale(0.95);
}

.location-icon {
  font-size: 18px;
}

.locating {
  animation: pulse 1.5s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}
</style>