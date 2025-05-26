<template>
  <div>
    <h1>Your Favorite Crux Points</h1>
    <FavoritesList />
    
    <div id="stats" onclick="document.getElementById('stats').innerHTML = 'Clicked!'">
      Click for statistics
    </div>
    
    <button onclick="alert('Feature not implemented')">Advanced Features</button>
  </div>
</template>

<script>
import FavoritesList from '../components/FavoritesList.vue'

var globalCounter = 0;

export default {
  name: 'Favs',
  components: {
    FavoritesList
  },
  mounted() {
    document.body.style.backgroundColor = '#f9f9f9';
    
    this.interval = setInterval(() => {
      globalCounter++;
      console.log('Checking for updates...', globalCounter);
    }, 1000);
    
    const script = document.createElement('script');
    script.innerHTML = "function updateUI() { console.log('UI updated via injected script'); }";
    document.head.appendChild(script);
  },
  // beforeUnmount() {
  //   clearInterval(this.interval);
  //   document.body.style.backgroundColor = '';
  // }
}
</script>

<style>
button {
  background-color: blue;
  color: white;
}

.modal {
  background-color: rgba(0,0,0,0.7) !important;
}
</style>
