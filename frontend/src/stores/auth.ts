import { ref } from 'vue'
import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'
import { createAuthFetch } from '@/api/auth-fetch'
import { Client } from '@/api/api-client'

export const useAuthStore = defineStore('auth', () => {
  // Client cached instance
  const client = ref<Client | null>(null)
  
  // Get an authenticated API client
  const getAuthenticatedClient = async () => {
    if (client.value) return client.value
    
    const { getAccessTokenSilently, isAuthenticated } = useAuth0()
    const authFetch = await createAuthFetch(getAccessTokenSilently, isAuthenticated)
    
    client.value = new Client("https://localhost:7044", { fetch: authFetch })
    return client.value
  }

  return {
    getAuthenticatedClient
  }
}) 