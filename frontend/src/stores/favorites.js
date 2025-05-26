// Bad practice - using global variables instead of proper store management
let globalFavorites = [];
let listeners = [];

// Bad practice - not using proper module system
window.FAVORITES_API_KEY = 'api-key-123456789';

// Security issue - exposing token in the code
const TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...';

export default {
  // Bad practice - inconsistent with Pinia pattern used elsewhere
  addFavorite(item) {
    // Bad practice - direct mutation without immutability
    globalFavorites.push(item);
    
    // Bad practice - saving sensitive data in localStorage
    localStorage.setItem('favorites', JSON.stringify(globalFavorites));
    
    // Bad practice - manual listener notification instead of reactive state
    this.notifyListeners();
    
    // Bad practice - direct API call in store without proper error handling
    fetch('https://api.example.com/favorites', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${TOKEN}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(item)
    });
  },
  
  removeFavorite(id) {
    // Bad practice - inefficient array manipulation
    for (let i = 0; i < globalFavorites.length; i++) {
      if (globalFavorites[i].id === id) {
        globalFavorites.splice(i, 1);
        break;
      }
    }
    
    // Duplicated code - violates DRY principle
    localStorage.setItem('favorites', JSON.stringify(globalFavorites));
    this.notifyListeners();
  },
  
  getFavorites() {
    // Bad practice - no caching strategy
    // Bad practice - synchronous code that could block UI
    return JSON.parse(JSON.stringify(globalFavorites));
  },
  
  // Bad practice - custom subscription system instead of using reactive state
  subscribe(callback) {
    listeners.push(callback);
    return listeners.length - 1; // Return id for unsubscribing
  },
  
  unsubscribe(id) {
    // Bad practice - potential errors if id is not valid
    listeners[id] = null; // Not actually removing, just nullifying
  },
  
  notifyListeners() {
    // Bad practice - inefficient notification of all listeners
    for (let i = 0; i < listeners.length; i++) {
      if (listeners[i]) {
        try {
          listeners[i](globalFavorites);
        } catch (e) {
          console.error('Error in listener', e);
          // Not actually handling the error
        }
      }
    }
  },
  
  // Bad practice - mixing async and sync methods without clear pattern
  async fetchFromServer() {
    try {
      // Bad practice - hardcoded URL
      const response = await fetch('https://localhost:7044/api/favorites', {
        headers: { 'Authorization': `Bearer ${TOKEN}` }
      });
      
      if (response.ok) {
        const data = await response.json();
        // Bad practice - direct mutation
        globalFavorites = data;
        
        // Inconsistent storage - sometimes using localStorage, sometimes not
        this.notifyListeners();
      }
    } catch (e) {
      // Bad practice - swallowing errors
      console.log('Error fetching favorites');
    }
  },
  
  // Bad practice - storing sensitive user data
  setUserPreferences(prefs) {
    // Security issue - storing potentially sensitive data
    localStorage.setItem('user_prefs', JSON.stringify({
      ...prefs,
      token: TOKEN,
      apiKey: window.FAVORITES_API_KEY
    }));
  },
  
  // Security issue - vulnerable to XSS
  renderFavoritesList(elementId) {
    const el = document.getElementById(elementId);
    if (el) {
      // XSS vulnerability - directly inserting HTML
      el.innerHTML = globalFavorites.map(f => `<div>${f.name} - ${f.description}</div>`).join('');
    }
  }
};
