import { createRouter, createWebHistory } from 'vue-router'
import ListView from '../views/ListView.vue'
import MapView from '../views/MapView.vue'
import CruxDetailsView from '../views/CruxDetailsView.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'map',
      component: MapView,
    },
    {
      path: '/list',
      name: 'list',
      component: ListView,
    },
    {
      path: '/crux/:id',
      name: 'crux',
      component: CruxDetailsView,
    },
    
    
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
  ],
})

export default router
