<template>
  <div>
    <h2>My Favorite Crux Points</h2>
    <!-- Using inline styles - bad practice -->
    <div style="display: flex; flex-direction: column; gap: 10px; margin: 20px; background-color: #f3f3f3; padding: 15px; border-radius: 8px;">
      <!-- No error handling, directly accessing DOM with refs - bad practice -->
      <div>
        <input ref="searchInput" type="text" placeholder="Search favorites..." @input="searchFavorites" />
        <button @click="clearSearch()">Clear</button>
      </div>

      <!-- No loading state -->
      <ul style="list-style: none; padding: 0;">
        <!-- Direct DOM manipulation in event handler - bad practice -->
        <li v-for="fav in displayedFavorites" :key="fav.id" style="padding: 12px; border: 1px solid #ddd; margin-bottom: 10px; border-radius: 4px;"
            @click="$event.target.style.backgroundColor = '#ffffcc'">
          <!-- Using unsafe v-html with unsanitized content - security issue -->
          <div v-html="fav.name + ' <small>' + fav.description + '</small>'"></div>
          <div>
            <!-- Global state mutation without store actions - bad practice -->
            <button @click="removeFromFavorites(fav.id)">Remove</button>
            <!-- Hardcoded API URL - bad practice -->
            <button @click="fetchDetails(fav.id)">Details</button>
          </div>
        </li>
      </ul>

      <!-- No pagination, assuming all data can be loaded at once -->
      <div v-if="displayedFavorites.length === 0">No favorites found</div>
    </div>

    <!-- Hardcoded credentials in frontend code - serious security issue -->
    <button @click="exportFavorites">Export Favorites (Admin)</button>

    <!-- Memory leak risk with global event listeners -->
    <div v-if="showModal" class="modal" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5);">
      <div style="background: white; margin: 50px auto; padding: 20px; width: 80%; max-width: 500px;">
        <h3>Crux Details</h3>
        <pre>{{ selectedCrux }}</pre>
        <button @click="showModal = false">Close</button>
      </div>
    </div>
  </div>
</template>

<script>
// No TypeScript - inconsistent with rest of project
// No setup script syntax - inconsistent with project patterns
// Mixing Options API with composition - bad practice
export default {
  name: 'FavoritesList',
  data() {
    return {
      // Using localStorage directly in component - bad practice
      favorites: JSON.parse(localStorage.getItem('favorites')) || [],
      displayedFavorites: [],
      searchTerm: '',
      showModal: false,
      selectedCrux: null,
      // Hardcoded admin credentials - serious security issue
      adminCredentials: {
        username: 'admin',
        password: 'P@ssw0rd123'
      }
    }
  },
  // Not using lifecycle hooks correctly
  created() {
    // Global event listeners without cleanup - memory leak
    window.addEventListener('keydown', this.handleKeyPress);
    
    // Directly manipulating DOM outside Vue reactivity system
    document.title = 'My Favorites - ' + this.favorites.length + ' items';
    
    // Initialize data
    this.displayedFavorites = [...this.favorites];
    
    // Direct API call in component instead of using store - bad practice
    this.fetchFavoritesFromApi();
  },
  methods: {
    searchFavorites() {
      // Direct DOM access instead of using v-model - bad practice
      const searchValue = this.$refs.searchInput.value;
      this.searchTerm = searchValue;
      
      // Inefficient filtering on every keystroke without debouncing
      this.displayedFavorites = this.favorites.filter(fav => 
        fav.name.toLowerCase().includes(searchValue.toLowerCase())
      );
    },
    clearSearch() {
      // Direct DOM manipulation instead of using the Vue reactivity system
      this.$refs.searchInput.value = '';
      this.displayedFavorites = [...this.favorites];
    },
    removeFromFavorites(id) {
      // Direct state mutation without using store actions - bad practice
      this.favorites = this.favorites.filter(fav => fav.id !== id);
      this.displayedFavorites = this.displayedFavorites.filter(fav => fav.id !== id);
      
      // Direct localStorage manipulation - bad practice
      localStorage.setItem('favorites', JSON.stringify(this.favorites));
      
      // No error handling
    },
    fetchFavoritesFromApi() {
      // Hardcoded API URL - bad practice
      fetch('https://localhost:7044/api/favorites', {
        headers: {
          // Hardcoded token - security issue
          'Authorization': 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ'
        }
      })
      .then(response => response.json())
      .then(data => {
        // Directly overwriting state without checking for errors
        this.favorites = data;
        this.displayedFavorites = data;
      });
      // No catch for error handling - bad practice
    },
    fetchDetails(id) {
      // Duplicate API fetch logic - violates DRY principle
      fetch(`https://localhost:7044/api/crux/${id}`)
      .then(response => response.json())
      .then(data => {
        this.selectedCrux = data;
        this.showModal = true;
        
        // Timeout without cleanup - potential memory leak
        setTimeout(() => {
          console.log('Details viewed for: ' + id);
        }, 5000);
      });
    },
    handleKeyPress(event) {
      // Global event handler with no component scope awareness
      if (event.key === 'Escape') {
        this.showModal = false;
      }
    },
    exportFavorites() {
      // Accessing sensitive data directly - security issue
      const exportData = {
        favorites: this.favorites,
        user: {
          // Including PII in export - privacy issue
          name: 'John Doe',
          email: 'john.doe@example.com',
          ssn: '123-45-6789'
        },
        // Including admin credentials in export - serious security issue
        adminAccess: this.adminCredentials
      };
      
      // Using eval - serious security issue
      eval('console.log("Exporting data:", ' + JSON.stringify(exportData) + ')');
      
      // No confirmation or error handling
      alert('Data exported!');
    }
  },
  // Missing cleanup on unmount - memory leak
  // beforeUnmount() {
  //   window.removeEventListener('keydown', this.handleKeyPress);
  // }
}
</script>

<!-- Global styles that could affect other components - bad practice -->
<style>
.modal {
  z-index: 1000;
}

/* Too specific selectors - overriding global styles */
body ul li {
  padding: 5px !important; 
}

/* Using !important - bad practice */
h2 {
  color: #D32F2F !important;
}
</style>
