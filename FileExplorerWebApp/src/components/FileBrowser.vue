<template>
  <div class="file-browser">
    <div class="table-container">
      <table class="file-table">
        <thead>
          <tr>
            <th class="navigation left-align" colspan="5">
              <button
                v-if="currentPath !== rootPath && fetchTriggered"
                class="back-btn"
                @click="navigateUp"
              >
                ◀︎ Back
              </button>
              <span class="current-path">{{ currentPathBreadCrumb }}</span>
            </th>
          </tr>
          <tr colspan="5" v-if="!rootPath" class="empty-folder-msg">
            Path not provided
          </tr>
          <tr v-else-if="folders.length">
            <th class="left-align" @click="sortBy('name')">
              Name
              <span v-if="sortColumn === 'name'">{{ sortDirection === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('size')">
              Size
              <span v-if="sortColumn === 'size'">{{ sortDirection === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('created')">
              Created
              <span v-if="sortColumn === 'created'">{{ sortDirection === 'asc' ? '▲' : '▼' }}</span>
            </th>
            <th @click="sortBy('modified')">
              Modified
              <span v-if="sortColumn === 'modified'">{{
                sortDirection === 'asc' ? '▲' : '▼'
              }}</span>
            </th>
            <th>Actions</th>
          </tr>
          <tr colspan="5" v-else class="empty-folder-msg">
            📂 Folder is empty
          </tr>
        </thead>
        <tbody>
          <FileRow
            v-for="(item, index) in sortedFiles"
            :key="index"
            :file="item"
            @open-folder="navigateToFolder"
          />
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { ref, computed, watch } from 'vue'
import FileRow from './FileRow.vue'
import apiClient from '@/api'
import '@/assets/css/file_browser.css'

export default {
  components: { FileRow },
  props: {
    rootPath: String,
    fetchTriggered: Boolean,
  },
  emits: ['fetch-complete'],
  setup(props, { emit }) {
    // Initialize currentPath with rootPath
    const currentPath = ref(props.rootPath)
    const currentPathBreadCrumb = ref()
    const folders = ref([])
    const sortColumn = ref('name')
    const sortDirection = ref('asc')

    // Fetch files based on the current path
    const fetchFiles = async (path) => {
      folders.value = [] // Reset on new path
      try {
        const response = await apiClient.get(
          `/api/DirectoryPath/directories?path=${encodeURIComponent(path)}`,
        )
        if (response.status == 200) {
          currentPathBreadCrumb.value = response.data.path.replace('/', '').replace(/\//g, ' > ')
          folders.value = response.data.children || []
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
      emit('fetch-complete')
    }

    // Watch for the fetchTriggered prop to trigger the fetch
    watch(
      () => props.fetchTriggered,
      (newVal) => {
        if (newVal) {
          currentPath.value = props.rootPath
          fetchFiles(props.rootPath)
        }
      },
    )

    const navigateToFolder = (folderName) => {
      currentPath.value = `${currentPath.value}/${folderName}`
      fetchFiles(currentPath.value)
    }

    const navigateUp = () => {
      if (currentPath.value !== props.rootPath) {
        currentPath.value = currentPath.value.substring(0, currentPath.value.lastIndexOf('/'))
        fetchFiles(currentPath.value)
      }
    }

    const sortedFiles = computed(() => {
      return [...folders.value].sort((a, b) => {
        let result = 0
        if (a.type !== b.type) return a.type === 'directory' ? -1 : 1
        if (sortColumn.value === 'name') {
          result = a.name.localeCompare(b.name)
        } else if (sortColumn.value === 'size') {
          result = (a.size || 0) - (b.size || 0)
        } else if (sortColumn.value === 'created') {
          result = new Date(a.creationDateTime || 0) - new Date(b.creationDateTime || 0)
        } else if (sortColumn.value === 'modified') {
          result = new Date(a.modifiedDateTime || 0) - new Date(b.modifiedDateTime || 0)
        }
        return sortDirection.value === 'asc' ? result : -result
      })
    })

    const sortBy = (column) => {
      if (sortColumn.value === column) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc'
      } else {
        sortColumn.value = column
        sortDirection.value = 'asc'
      }
    }

    return {
      currentPath,
      currentPathBreadCrumb,
      folders,
      sortedFiles,
      navigateToFolder,
      navigateUp,
      sortBy,
      sortColumn,
      sortDirection,
    }
  },
}
</script>
