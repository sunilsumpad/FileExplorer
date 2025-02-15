<template>
  <div class="app">
    <header class="header">📁 File Browser</header>

    <div class="input-container">
      <input v-model="path" placeholder="Enter a path (e.g., /home/user)" />
      <button class="default-btn-class" @click="sendPath">Browse</button>
    </div>
    <FileBrowser :path="sentPath"/>
  </div>
</template>

<script>
import '@/assets/css/app.css'
import FileBrowser from './components/FileBrowser.vue'

export default {
  components: { FileBrowser },
  data() {
    return {
      path: '',
      sentPath: '',
    }
  },
  methods: {
    sendPath() {
      if (this.path) {
        this.sentPath = this.path
      }
      else {
        alert('Please enter the path')
      }
    }
  },
}
</script>
