<script setup lang="ts">
import { useCruxStore } from '@/stores/crux';

const cruxStore = useCruxStore()

// Function to format crux level as stars
const formatLevel = (level?: number) => {
  if (!level) return '';
  const stars = '★'.repeat(level);
  return stars;
};
</script>
<template>
  <h1>Crux list</h1>
  <main>
    <div class="crux-list">
      <h2>Crux Points</h2>
      <ul>
        <li v-for="crux in cruxStore.items" :key="crux.id" class="crux-item"
          @click="center = [parseFloat(crux.lat || '0'), parseFloat(crux.lng || '0')]; zoom = 15;">
          <div class="crux-item-name">{{ crux.name }}</div>
          <div class="crux-item-details">
            <span v-if="crux.level" class="difficulty-indicator">{{ formatLevel(crux.level) }}</span>
            <span class="coordinates">({{ crux.lat }}, {{ crux.lng }})</span>
          </div>
        </li>
      </ul>
    </div>
  </main>
</template>