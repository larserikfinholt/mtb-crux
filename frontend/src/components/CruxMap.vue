<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { useCruxStore } from '@/stores/crux';
import 'leaflet/dist/leaflet.css';
import { LMap, LTileLayer, LMarker, LPopup, LIcon } from '@vue-leaflet/vue-leaflet';
import { useRouter } from 'vue-router';

const cruxStore = useCruxStore();
const zoom = ref(13);
const center = ref([59.9139, 10.7522]); // Default center (Oslo)
const mapReady = ref(false);
const markerIcon = ref(null);
const router = useRouter();

// Function to format crux level as stars
const formatLevel = (level?: number) => {
  if (!level) return '';
  const stars = '★'.repeat(level);
  return stars;
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
</script>

<template>
  <div class="crux-map-container">
    <h1 class="crux-title">Crux Map</h1>
    
    <div class="map-wrapper">
      <l-map
        v-model:zoom="zoom"
        :center="center"
        class="map"
      >
        <l-tile-layer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          layer-type="base"
          name="OpenStreetMap"
          attribution="&copy; <a href='https://www.openstreetmap.org/copyright'>OpenStreetMap</a> contributors"
        ></l-tile-layer>
        
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
    </div>
    
    
  </div>
</template>

<style scoped>
.crux-map-container {
  display: flex;
  flex-direction: column;
  height: 100%;
  max-width: 100%;
  padding: 0.5rem;
}

.crux-title {
  margin-bottom: 1rem;
  font-size: 1.5rem;
  font-weight: bold;
}

.map-wrapper {
  width: 100%;
  height: 60vh;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.map {
  width: 100%;
  height: 100%;
}

.crux-list {
  margin-top: 1.5rem;
  border-radius: 8px;
  padding: 1rem;
  background-color: #f9f9f9;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  max-height: 300px;
  overflow-y: auto;
}

.crux-list h2 {
  margin-top: 0;
  margin-bottom: 0.75rem;
  font-size: 1.25rem;
}

.crux-list ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.crux-item {
  padding: 0.75rem;
  border-bottom: 1px solid #eee;
  cursor: pointer;
  transition: background-color 0.2s;
}

.crux-item:last-child {
  border-bottom: none;
}

.crux-item:hover {
  background-color: #f0f0f0;
}

.crux-item-name {
  font-weight: bold;
  margin-bottom: 0.25rem;
}

.crux-item-details {
  display: flex;
  justify-content: space-between;
  font-size: 0.85rem;
  color: #666;
}

.difficulty-indicator {
  color: #f0ad4e;
}

.coordinates {
  font-family: monospace;
  font-size: 0.8rem;
}

.popup-content {
  padding: 0.5rem;
}

.popup-content h3 {
  margin-top: 0;
  margin-bottom: 0.5rem;
}

.difficulty {
  margin-bottom: 0.5rem;
}

.stars {
  color: #f0ad4e;
}

/* Mobile responsive styles */
@media (max-width: 768px) {
  .crux-map-container {
    padding: 0.25rem;
  }
  
  .crux-title {
    font-size: 1.25rem;
    text-align: center;
  }
  
  .map-wrapper {
    height: 50vh;
  }
  
  .crux-list {
    margin-top: 1rem;
    padding: 0.75rem;
  }
  
  .crux-list h2 {
    font-size: 1.1rem;
    text-align: center;
  }
  
  .crux-item {
    padding: 0.5rem;
  }
}
</style>