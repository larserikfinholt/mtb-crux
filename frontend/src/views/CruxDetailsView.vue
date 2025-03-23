<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useCruxStore } from '@/stores/crux';
import { type Crux } from '@/api/api-client';
import { useRoute } from 'vue-router';
import CruxMap from '@/components/CruxMap.vue';

// load the crux details from the api, use route params to get the id
const route = useRoute();
const cruxStore = useCruxStore();
// Fix linter error - ensure it's a string before parsing
const id = parseInt(route.params.id as string);

const crux = ref<Crux>();

onMounted(async () => {
    if (cruxStore.items.length === 0) {
        await cruxStore.fetchItems();
    }

    crux.value = cruxStore.items.find(c => c.id === id);
});

// Computed property to get the coordinates for the map
const cruxCoordinates = computed(() => {
    if (crux.value?.lat && crux.value?.lng) {
        return [parseFloat(crux.value.lat), parseFloat(crux.value.lng)] as [number, number];
    }
    return undefined;
});
</script>

<template>
  <div class="crux-details">
    <div class="details-container">
      <h1>{{ crux?.name }}</h1>
      <p class="description">{{ crux?.description }}</p>
      
      <!-- Additional details can be added here -->
    </div>
    
    <div class="map-container" v-if="crux && cruxCoordinates">
      <CruxMap 
        :showSingleCrux="true" 
        :cruxId="id" 
        :initialCenter="cruxCoordinates"
        :mapHeight="'400px'"
      />
    </div>
  </div>
</template>

<style scoped>
.crux-details {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

.details-container {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.description {
  margin-top: 10px;
  line-height: 1.6;
}

.map-container {
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

@media (min-width: 768px) {
  .crux-details {
    flex-direction: row;
  }
  
  .details-container {
    flex: 1;
  }
  
  .map-container {
    flex: 1;
  }
}
</style>