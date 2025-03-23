<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';
import { useRouter } from 'vue-router';
import { Crux } from '@/api/api-client';
import { getAuthenticatedClient } from '@/api/auth-helper';
import EditableCruxMap from '@/components/EditableCruxMap.vue';

// Check if user is authenticated
const { isAuthenticated, loginWithRedirect } = useAuth0();
const router = useRouter();

// Redirect to login if not authenticated
onMounted(() => {
  if (!isAuthenticated.value) {
    loginWithRedirect({
      appState: { targetUrl: window.location.pathname }
    });
  }
});

// Form data using the Crux constructor properly
const newCrux = ref(new Crux({
  name: '',
  description: '',
  level: 1,
  lat: '',
  lng: ''
}));

// Form validation
const errors = ref({
  name: '',
  position: ''
});

const selectedPosition = ref<[number, number] | null>(null);
const isSubmitting = ref(false);

// Function to handle position selection from the map
const handlePositionSelected = (position: [number, number]) => {
  selectedPosition.value = position;
  newCrux.value.lat = position[0].toString();
  newCrux.value.lng = position[1].toString();
  errors.value.position = '';
};

// Computed property for map coordinates
const mapCoordinates = computed(() => {
  return selectedPosition.value || [59.9139, 10.7522] as [number, number];
});

// Function to save the new crux
const saveCrux = async () => {
  // Reset errors
  errors.value.name = '';
  errors.value.position = '';
  
  // Validate form
  let isValid = true;
  
  if (!newCrux.value.name?.trim()) {
    errors.value.name = 'Title is required';
    isValid = false;
  }
  
  if (!newCrux.value.lat || !newCrux.value.lng) {
    errors.value.position = 'Please select a position on the map';
    isValid = false;
  }
  
  if (!isValid) return;
  
  try {
    isSubmitting.value = true;
    
    // Check if user is authenticated
    if (!isAuthenticated.value) {
      await loginWithRedirect({
        appState: { targetUrl: window.location.pathname }
      });
      return;
    }

    // Get authenticated client
    const client = await getAuthenticatedClient();
    
    // Create new crux
    await client.createCrux(newCrux.value);
    
    // Navigate to map view after successful creation
    router.push('/map');
  } catch (error) {
    console.error('Error creating crux:', error);
    alert('Failed to create new crux. Please try again.');
  } finally {
    isSubmitting.value = false;
  }
};
</script>

<template>
  <div v-if="isAuthenticated" class="new-crux-page">
    <h1>Add New Crux</h1>
    
    <form @submit.prevent="saveCrux" class="crux-form">
      <div class="form-group">
        <label for="name">Title *</label>
        <input 
          id="name" 
          v-model="newCrux.name" 
          type="text" 
          placeholder="Enter crux title"
          :class="{ 'error-input': errors.name }"
          required
        >
        <p v-if="errors.name" class="error-message">{{ errors.name }}</p>
      </div>
      
      <div class="form-group">
        <label for="description">Description</label>
        <textarea 
          id="description" 
          v-model="newCrux.description" 
          placeholder="Enter crux description"
          rows="4"
        ></textarea>
      </div>
      
      <div class="form-group">
        <label for="level">Difficulty Level</label>
        <select id="level" v-model="newCrux.level">
          <option :value="1">★</option>
          <option :value="2">★★</option>
          <option :value="3">★★★</option>
          <option :value="4">★★★★</option>
          <option :value="5">★★★★★</option>
        </select>
      </div>
      
      <div class="form-group position-group">
        <label>Position *</label>
        <p class="help-text">Click on the map to set the position</p>
        <p v-if="errors.position" class="error-message">{{ errors.position }}</p>
        
        <div v-if="selectedPosition" class="selected-coordinates">
          <p>Latitude: {{ selectedPosition[0].toFixed(6) }}</p>
          <p>Longitude: {{ selectedPosition[1].toFixed(6) }}</p>
        </div>
      </div>
      
      <button 
        type="submit" 
        class="submit-button"
        :disabled="isSubmitting"
      >
        {{ isSubmitting ? 'Saving...' : 'Save Crux' }}
      </button>
    </form>
    
    <div class="map-container">
      <EditableCruxMap 
        :mapHeight="'400px'"
        :initialCenter="mapCoordinates"
        :selectedPosition="selectedPosition"
        @position-selected="handlePositionSelected"
      />
    </div>
  </div>
  <div v-else class="loading">
    <p>Please wait, redirecting to login...</p>
  </div>
</template>

<style scoped>
.new-crux-page {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

h1 {
  margin-bottom: 20px;
  font-size: 24px;
}

.crux-form {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.form-group {
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
}

input, textarea, select {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
}

input:focus, textarea:focus, select:focus {
  outline: none;
  border-color: #4CAF50;
}

.error-input {
  border-color: #f44336;
}

.error-message {
  color: #f44336;
  font-size: 12px;
  margin-top: 4px;
}

.help-text {
  color: #666;
  font-size: 12px;
  margin-top: 4px;
}

.submit-button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
  font-weight: 500;
  transition: background-color 0.3s;
  width: 100%;
}

.submit-button:hover {
  background-color: #45a049;
}

.submit-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.map-container {
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.selected-coordinates {
  background-color: #f5f5f5;
  padding: 8px 12px;
  border-radius: 4px;
  margin-top: 8px;
  font-size: 14px;
}

.selected-coordinates p {
  margin: 4px 0;
}

.loading {
  padding: 40px;
  text-align: center;
  font-size: 18px;
  color: #666;
}

@media (min-width: 768px) {
  .new-crux-page {
    flex-direction: row;
  }
  
  .crux-form {
    flex: 1;
    max-width: 500px;
  }
  
  .map-container {
    flex: 1;
  }
}
</style> 