<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useCruxStore } from '@/stores/crux';
import { onMounted } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';

const { loginWithRedirect, user, isAuthenticated } = useAuth0();
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
        
        <button v-if="!isAuthenticated" @click="loginWithRedirect()">Log in</button>
        <span v-else>{{ user?.name }}</span>
      </nav>
  </header>

  <RouterView />
</template>

