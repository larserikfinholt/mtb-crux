<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useCruxStore } from '@/stores/crux';
import { onMounted } from 'vue';


const cruxStore = useCruxStore()

onMounted(() => {
  console.log('fetching items')
  cruxStore.fetchItems()
})
</script>

<template>
  <header>
      <nav>
        <RouterLink to="/">Sykkelcrux</RouterLink>
        <RouterLink to="/map">Map</RouterLink>
        <RouterLink to="/list">List</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>
  </header>

  <RouterView />
</template>

