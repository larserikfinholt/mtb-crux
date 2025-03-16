<script setup lang="ts">
import { useCruxStore } from '@/stores/crux';
import { onMounted } from 'vue';


const cruxStore = useCruxStore()

onMounted(() => {
  cruxStore.fetchItems()
})

</script>
<template>
  <main>
    <ul>
      <li v-for="item in cruxStore.items" :key="item.id">
        {{ item.name }}
      </li>
    </ul>
  </main>
</template>