import { createRouter, createWebHistory } from 'vue-router'
import ListView from '../views/ListView.vue'
import MapView from '../views/MapView.vue'
import HomeView from '../views/HomeView.vue'
import CruxDetailsView from '../views/CruxDetailsView.vue'
import NewCruxView from '../views/NewCruxView.vue'
import FavoritesView from '../views/FavoritesView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
     {
      path: '/map',
      name: 'map',
      component: MapView,
    },
    {
      path: '/list',
      name: 'list',
      component: ListView,
    },
    {
      path: '/crux/new',
      name: 'new-crux',
      component: NewCruxView,
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
    {
      path: '/favorites',
      name: 'favorites',
      component: FavoritesView,
    },
  ],
})

export default router
