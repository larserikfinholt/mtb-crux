<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import { useCruxStore } from '@/stores/crux';
import 'leaflet/dist/leaflet.css';
import { LMap, LTileLayer, LMarker, LPopup, LIcon, LLayerGroup, LControl } from '@vue-leaflet/vue-leaflet';
import { useRouter } from 'vue-router';

// Define custom events
const emit = defineEmits(['position-selected']);

// Props for the component
const props = defineProps({
  initialCenter: {
    type: Array as unknown as () => [number, number] | undefined,
    default: undefined
  },
  mapHeight: {
    type: String,
    default: '400px'
  },
  selectedPosition: {
    type: Array as unknown as () => [number, number] | null,
    default: null
  }
});

// State variables
const zoom = ref(13);
const center = ref(props.initialCenter || [59.9139, 10.7522] as [number, number]);
const mapReady = ref(false);
const markerIcon = ref<any>(null);
const mapInstance = ref<any>(null);
const activeBaseLayer = ref('norgeskart-topo');
const isLayerSelectorVisible = ref(false);
const isLocating = ref(false);

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

// Function to toggle layer selector visibility
const toggleLayerSelector = () => {
  isLayerSelectorVisible.value = !isLayerSelectorVisible.value;
};

// Function to change the base layer
const changeBaseLayer = (layerId: string) => {
  activeBaseLayer.value = layerId;
  isLayerSelectorVisible.value = false;
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

// Handle map click to select position
const handleMapClick = (e: any) => {
  const position: [number, number] = [e.latlng.lat, e.latlng.lng];
  console.log('Map clicked at position:', position);
  emit('position-selected', position);
};

onMounted(async () => {
  // Use initial center from props if available
  if (props.initialCenter) {
    center.value = props.initialCenter;
  }
  // Try to get user location if not specified
  else if (navigator.geolocation) {
    try {
      const position = await new Promise<GeolocationPosition>((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject, { 
          timeout: 5000,
          maximumAge: 60000 
        });
      });
      
      const { latitude, longitude } = position.coords;
      center.value = [latitude, longitude];
    } catch (error) {
      console.warn('Could not get user location, using default coordinates', error);
    }
  }

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
  
  mapReady.value = true;
});

// Store map reference when ready
const onMapReady = (map: any) => {
  mapInstance.value = map;
  // Add click handler for position selection
  if (map) {
    map.on('click', handleMapClick);
  }
};

// Watch for changes in selected position from parent
watch(() => props.selectedPosition, (newPosition) => {
  if (newPosition) {
    center.value = newPosition;
  }
}, { immediate: true });
</script>

<template>
  <div class="map-wrapper" :style="{ height: mapHeight }">
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
      
      <!-- Display marker for selected position -->
      <l-marker 
        v-if="selectedPosition"
        :lat-lng="selectedPosition"
      ></l-marker>
    </l-map>
    
    <!-- Layer control - only visible when toggled -->
    <div class="layer-control" v-if="isLayerSelectorVisible">
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
    
    <!-- Layer toggle button -->
    <button 
      class="layer-toggle-button" 
      @click="toggleLayerSelector"
      title="Change map style"
    >
      <span class="layer-icon">🗺️</span>
    </button>
    
    <!-- Location button -->
    <button 
      class="location-button" 
      @click="getCurrentLocation"
      :class="{ 'locating': isLocating }"
      title="Find my location"
    >
      <span class="location-icon">📍</span>
    </button>
    
    <!-- Instruction overlay -->
    <div class="instruction-overlay">
      Click on the map to select a position
    </div>
  </div>
</template>

<style scoped>
.map-wrapper {
  width: 100%;
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
  bottom: 70px;
  right: 20px;
  z-index: 1000;
  background: white;
  border-radius: 4px;
  box-shadow: 0 1px 5px rgba(0,0,0,0.4);
  padding: 6px;
  display: flex;
  flex-direction: column;
  max-width: 200px;
  animation: fadeIn 0.2s ease;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
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

.layer-toggle-button {
  position: absolute;
  bottom: 70px;
  right: 20px;
  z-index: 999;
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

.layer-toggle-button:hover {
  background-color: #f0f0f0;
}

.layer-toggle-button:active {
  transform: scale(0.95);
}

.layer-icon {
  font-size: 18px;
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

.instruction-overlay {
  position: absolute;
  top: 10px;
  left: 50%;
  transform: translateX(-50%);
  background-color: rgba(255, 255, 255, 0.8);
  padding: 8px 16px;
  border-radius: 20px;
  font-size: 14px;
  box-shadow: 0 1px 5px rgba(0,0,0,0.2);
  pointer-events: none;
  z-index: 1000;
}
</style> 