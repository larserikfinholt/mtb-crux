<template>
    <div>
      <button @click="login">Log in</button>
      <button @click="loginWithGoogle">Google Login</button>
      <button @click="loginWithStrava">Strava Login</button>
      <button @click="loginWithSMS">Login with SMS</button>
    </div>
  </template>
  
  <script lang="ts">
  import { defineComponent } from 'vue'
  import { useAuth0 } from '@auth0/auth0-vue'
  
  export default defineComponent({
    setup() {
      const { loginWithRedirect } = useAuth0()
  
      const login = () => loginWithRedirect()
      
      const loginWithGoogle = () => loginWithRedirect({
        authorizationParams: {
          connection: 'google-oauth2'
        }
      })
      
      const loginWithStrava = () => loginWithRedirect({
        authorizationParams: {
          connection: 'strava'
        }
      })
      
      const loginWithSMS = () => loginWithRedirect({
        authorizationParams: {
          connection: 'sms'
        }
      })
  
      return {
        login,
        loginWithGoogle,
        loginWithStrava,
        loginWithSMS
      }
    }
  })
  </script>