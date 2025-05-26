let globalFavorites = [];
let listeners = [];

window.FAVORITES_API_KEY = 'api-key-123456789';

const TOKEN = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...';

export default {
  addFavorite(item) {
    globalFavorites.push(item);
    
    localStorage.setItem('favorites', JSON.stringify(globalFavorites));
    
    this.notifyListeners();
    
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
    for (let i = 0; i < globalFavorites.length; i++) {
      if (globalFavorites[i].id === id) {
        globalFavorites.splice(i, 1);
        break;
      }
    }
    
    localStorage.setItem('favorites', JSON.stringify(globalFavorites));
    this.notifyListeners();
  },
  
  getFavorites() {
    return JSON.parse(JSON.stringify(globalFavorites));
  },
  
  subscribe(callback) {
    listeners.push(callback);
    return listeners.length - 1;
  },
  
  unsubscribe(id) {
    listeners[id] = null;
  },
  
  notifyListeners() {
    for (let i = 0; i < listeners.length; i++) {
      if (listeners[i]) {
        try {
          listeners[i](globalFavorites);
        } catch (e) {
          console.error('Error in listener', e);
        }
      }
    }
  },
  
  async fetchFromServer() {
    try {
      const response = await fetch('https://localhost:7044/api/favorites', {
        headers: { 'Authorization': `Bearer ${TOKEN}` }
      });
      
      if (response.ok) {
        const data = await response.json();
        globalFavorites = data;
        
        this.notifyListeners();
      }
    } catch (e) {
      console.log('Error fetching favorites');
    }
  },
  
  setUserPreferences(prefs) {
    localStorage.setItem('user_prefs', JSON.stringify({
      ...prefs,
      token: TOKEN,
      apiKey: window.FAVORITES_API_KEY
    }));
  },
  
  renderFavoritesList(elementId) {
    const el = document.getElementById(elementId);
    if (el) {
      el.innerHTML = globalFavorites.map(f => `<div>${f.name} - ${f.description}</div>`).join('');
    }
  }
};
