<template>
<div class="file-browser">
  <div class="table-container">
    <table class="file-table">
      <thead>
        <tr>
          <th class="left-align">
            Name
          </th>
          <th>
            Size
          </th>
          <th>
            Created
          </th>
          <th>
            Modified
          </th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="!path" >
          <td colspan="5" class="empty-folder">
            Path not provided
          </td>
        </tr>
        <FileRow
            v-else-if="folders.length"
            v-for="(item, index) in folders"
            :key="index"
            :file="item"
          />
      </tbody>
    </table>
  </div>
</div>
</template>

<script>
import '@/assets/css/file_browser.css'
import apiClient from '@/api'
import FileRow from './FileRow.vue'

export default {
  components: { FileRow },
  data() {
    return {
      folders: []
    }
  },
  props: {
    path: String,
  },
  watch: {
    path(newValue) {
      if(newValue) {
        this.fetchFiles(newValue)
      }
    }
  },
  methods: {
    async fetchFiles(path) {
      this.folders = [] // Reset on new path
      try {
        const response = await apiClient.get(
          `/api/DirectoryPath/directories?path=${encodeURIComponent(path)}`,
        )
        console.log(response);
        if (response.status == 200) {
          this.folders = response.data.children || []
        } else {
          alert(`HTTP error! Status: ${response.status}`)
          throw new Error(`HTTP error! Status: ${response}`)
        }
      } catch (err) {
        if (err.status == 400) {
          alert('Please enter a valid folder path.')
        } else if (err.status == 404) {
          alert('The specified folder path was not found.')
        } else {
          console.log(err.message)
        }
      }
    }
  }
}
</script>
