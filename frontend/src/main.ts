import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { createAuth0 } from '@auth0/auth0-vue'

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.use(
    createAuth0({
      domain: import.meta.env.VITE_AUTH0_DOMAIN,
      clientId: import.meta.env.VITE_AUTH0_CLIENT_ID,
      authorizationParams: {
        redirect_uri: window.location.origin,
        audience: import.meta.env.VITE_AUTH0_AUDIENCE, // This should be your API identifier in Auth0
        scope: 'openid profile email' // Add email scope to request email claim
      }
    })
  );

app.mount('#app')
