<script setup lang="ts">
import { useCruxStore } from '@/stores/crux';

const cruxStore = useCruxStore()

</script>
<template>
    <h1>Crux map</h1>
  <div class="crux-map">
   a map
  </div>
  <ul>
      <li v-for="item in cruxStore.items" :key="item.id">
        {{ item.name }}
      </li>
    </ul>
</template>