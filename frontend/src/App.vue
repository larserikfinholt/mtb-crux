<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { useCruxStore } from '@/stores/crux';
import { useAuthStore } from '@/stores/auth';
import { onMounted, watch } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';

const { loginWithRedirect, user, isAuthenticated } = useAuth0();
const cruxStore = useCruxStore();
const authStore = useAuthStore();

onMounted(() => {
  console.log('fetching items')
  cruxStore.fetchItems()
})

// Watch for authentication state changes and register the user when authenticated
watch(isAuthenticated, (isAuth) => {
  if (isAuth) {
    console.log('User authenticated, registering with backend')
    authStore.registerUser()
  }
}, { immediate: true })
</script>

<template>
  <header>
      <nav>
        <RouterLink to="/">Sykkelcrux</RouterLink>
        <RouterLink to="/map">Map</RouterLink>
        <RouterLink to="/list">List</RouterLink>
        <RouterLink to="/about">About</RouterLink>
        
        <RouterLink v-if="isAuthenticated" to="/crux/new" class="add-crux-link">Add New Crux</RouterLink>
        
        <button v-if="!isAuthenticated" @click="loginWithRedirect()">Log in</button>
        <span v-else>{{ user?.name }}</span>
      </nav>
  </header>

  <RouterView />
</template>

<style>
nav {
  display: flex;
  align-items: center;
  padding: 1rem;
  gap: 1rem;
}

nav a {
  text-decoration: none;
  color: #333;
  padding: 0.5rem;
}

nav a.router-link-active {
  font-weight: bold;
  color: #4CAF50;
}

.add-crux-link {
  margin-left: auto;
  background-color: #4CAF50;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.add-crux-link:hover {
  background-color: #45a049;
}
</style>

