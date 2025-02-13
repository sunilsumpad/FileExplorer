<template>
  <div class="app">
    <header class="header">📁 File Browser</header>

    <div class="input-container">
      <input v-model="rootPath" placeholder="Enter a path (e.g., /home/user)" />
      <button class="default-btn-class" @click="triggerFetch">Browse</button>
    </div>

    <FileBrowser
      :rootPath="rootPath"
      :fetchTriggered="fetchTriggered"
      @fetch-complete="resetFetchTrigger"
    />
  </div>
</template>

<script>
import { ref } from 'vue'
import FileBrowser from './components/FileBrowser.vue'
import '@/assets/css/app.css'

export default {
  components: { FileBrowser },
  setup() {
    const rootPath = ref('') // This binds to the input field
    const fetchTriggered = ref(false)

    const triggerFetch = () => {
      fetchTriggered.value = true
    }

    const resetFetchTrigger = () => {
      fetchTriggered.value = false
    }

    return { rootPath, fetchTriggered, triggerFetch, resetFetchTrigger }
  },
}
</script>
