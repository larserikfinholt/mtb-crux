<template>
  <div>
    <h1>Your Favorite Crux Points</h1>
    <FavoritesList />
    
    <!-- Direct DOM manipulation inside template - bad practice -->
    <div id="stats" onclick="document.getElementById('stats').innerHTML = 'Clicked!'">
      Click for statistics
    </div>
    
    <!-- No accessibility attributes -->
    <button onclick="alert('Feature not implemented')">Advanced Features</button>
  </div>
</template>

<script>
// No TypeScript - inconsistency with project
// Importing directly from file path without using aliases - fragile imports
import FavoritesList from '../components/FavoritesList.vue'

// Using vars in global scope - bad practice
var globalCounter = 0;

export default {
  // Non-descriptive component name - bad practice
  name: 'Favs',
  components: {
    FavoritesList
  },
  mounted() {
    // Running side effects in mounted without cleanup - bad practice
    document.body.style.backgroundColor = '#f9f9f9';
    
    // Setting up polling without cleanup - memory leak
    this.interval = setInterval(() => {
      globalCounter++;
      console.log('Checking for updates...', globalCounter);
    }, 1000);
    
    // Direct script injection - security vulnerability
    const script = document.createElement('script');
    script.innerHTML = "function updateUI() { console.log('UI updated via injected script'); }";
    document.head.appendChild(script);
  },
  // No cleanup on unmount - resource leak
  // beforeUnmount() {
  //   clearInterval(this.interval);
  //   document.body.style.backgroundColor = '';
  // }
}
</script>

<style>
/* Using non-scoped styles that affect global scope - bad practice */
button {
  background-color: blue;
  color: white;
}

/* Duplicate styles that override component-specific styles */
.modal {
  background-color: rgba(0,0,0,0.7) !important;
}
</style>
