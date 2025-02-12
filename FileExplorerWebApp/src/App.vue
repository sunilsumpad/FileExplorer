<template>
  <div class="app">
    <header class="header">📁 File Browser</header>

    <div class="input-container">
      <input v-model="rootPath" placeholder="Enter a path (e.g., /home/user)" />
      <button @click="fetchFiles">Browse</button>
    </div>

    <!-- Scrollable Container -->
    <div class="table-container">
      <table class="file-table">
        <tbody>
          <file-row v-for="(folder, index) in folders" :key="index" :folder="folder" :depth="1" />
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue'
import FileRow from './components/FileRow.vue'
import '@/assets/css/app.css'
import apiClient from '@/api'

export default {
  components: { FileRow },
  setup() {
    const rootPath = ref('/Users/sunilpadmanabhan/Work/ASPDotNetCore/SunilDev/')
    const folders = ref([])

    const fetchFiles = async () => {
      folders.value = [] // Reset on new path
      try {
        const response = await apiClient.get(
          `/api/DirectoryPath/directories?path=${encodeURIComponent(rootPath.value)}`,
        )
        if (response.status >= 200 && response.status < 300) {
          const data = await response.data
          folders.value = data.children || []
        } else {
          throw new Error(`HTTP error! Status: ${response.status}`)
        }
      } catch (err) {
        console.error(err.message)
      }
    }
    return { rootPath, folders, fetchFiles }
  },
}
</script>
