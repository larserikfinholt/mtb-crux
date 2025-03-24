import { ref } from 'vue'
import { defineStore } from 'pinia'
import { useAuth0 } from '@auth0/auth0-vue'
import { createAuthFetch } from '@/api/auth-fetch'
import { Client } from '@/api/api-client'

export const useAuthStore = defineStore('auth', () => {
  // Client cached instance
  const client = ref<Client | null>(null)
  
  // Get Auth0 functions at the top level
  const auth0 = useAuth0()
  
  // Get an authenticated API client
  const getAuthenticatedClient = async () => {
    if (client.value) return client.value
    
    const authFetch = await createAuthFetch(
      () => auth0.getAccessTokenSilently(), 
      auth0.isAuthenticated
    )
    
    client.value = new Client("https://localhost:7044", { fetch: authFetch })
    return client.value
  }

  // Register the user with our backend after login
  const registerUser = async () => {
    try {
      if (!auth0.isAuthenticated.value) return
      
      // Get token
      const token = await auth0.getAccessTokenSilently()
      
      // Call our new register endpoint
      const response = await fetch('https://localhost:7044/auth/register', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      })
      
      if (!response.ok) {
        console.error('Failed to register user:', await response.text())
      } else {
        console.log('User registered successfully')
      }
    } catch (error) {
      console.error('Error registering user:', error)
    }
  }

  return {
    getAuthenticatedClient,
    registerUser
  }
}) 