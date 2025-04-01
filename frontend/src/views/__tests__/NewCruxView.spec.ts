import { describe, it, expect, vi, beforeEach } from 'vitest';
import { mount, flushPromises } from '@vue/test-utils';
import { createRouter, createWebHistory } from 'vue-router';
import NewCruxView from '../NewCruxView.vue';
import EditableCruxMap from '@/components/EditableCruxMap.vue';
import { Crux } from '@/api/api-client';

// Mock the auth0-vue module
vi.mock('@auth0/auth0-vue', () => ({
  useAuth0: () => ({
    isAuthenticated: { value: true },
    loginWithRedirect: vi.fn()
  })
}));

// Mock the api helper
vi.mock('@/api/auth-helper', () => ({
  getAuthenticatedClient: vi.fn().mockResolvedValue({
    createCrux: vi.fn().mockResolvedValue('new-crux-id')
  })
}));

// Create router for testing
const router = createRouter({
  history: createWebHistory(),
  routes: [{ path: '/map', name: 'map', component: { template: '<div>Map</div>' } }]
});

describe('NewCruxView', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders correctly when authenticated', () => {
    const wrapper = mount(NewCruxView, {
      global: {
        plugins: [router],
        stubs: {
          EditableCruxMap: true
        }
      }
    });

    expect(wrapper.find('h1').text()).toBe('Add New Crux');
    expect(wrapper.findComponent(EditableCruxMap).exists()).toBe(true);
  });

  it('validates form when required fields are missing', async () => {
    const wrapper = mount(NewCruxView, {
      global: {
        plugins: [router],
        stubs: {
          EditableCruxMap: true
        }
      }
    });

    // Try to submit with empty fields
    await wrapper.find('form').trigger('submit');
    
    // Check error messages
    expect(wrapper.find('.error-message').text()).toContain('Title is required');
    expect(wrapper.findAll('.error-message')[1].text()).toContain('Please select a position on the map');
  });

  it('updates position when map selection occurs', async () => {
    const wrapper = mount(NewCruxView, {
      global: {
        plugins: [router],
        stubs: {
          EditableCruxMap: true
        }
      }
    });

    // Simulate position selection from map
    const position: [number, number] = [60.123456, 10.654321];
    wrapper.findComponent(EditableCruxMap).vm.$emit('position-selected', position);
    await wrapper.vm.$nextTick();

    // Verify position was updated
    const vm = wrapper.vm as any;
    expect(vm.selectedPosition).toEqual(position);
    expect(vm.newCrux.lat).toBe(position[0].toString());
    expect(vm.newCrux.lng).toBe(position[1].toString());
    
    // Check that coordinates are displayed
    const coordsText = wrapper.find('.selected-coordinates').text();
    expect(coordsText).toContain('60.123456');
    expect(coordsText).toContain('10.654321');
  });

  it('submits the form and redirects when valid', async () => {
    const wrapper = mount(NewCruxView, {
      global: {
        plugins: [router],
        stubs: {
          EditableCruxMap: true
        }
      }
    });

    // Fill the form
    await wrapper.find('#name').setValue('Test Crux');
    await wrapper.find('#description').setValue('This is a test crux');
    await wrapper.find('#level').setValue(3);

    // Set position
    const position: [number, number] = [60.123456, 10.654321];
    wrapper.findComponent(EditableCruxMap).vm.$emit('position-selected', position);
    
    // Mock router push
    const routerPushSpy = vi.spyOn(router, 'push');

    // Submit the form
    await wrapper.find('form').trigger('submit');
    await flushPromises();

    // Verify that it tried to navigate to map view
    expect(routerPushSpy).toHaveBeenCalledWith('/map');
  });
});
