<template>
  <div class="file-browser">
    <div class="table-container">
      <table class="file-table">
        <thead>
          <tr>
            <th v-if="currentPath !== path" colspan="5" class="left-align">
              <button class="back-btn" @click="navigateToParentFolder">
                {{ UI_ICONS.BACK }} {{ UI_LABELS.BACK_BUTTON }}
              </button>
              {{ currentPathBreadCrumb }}
            </th>
          </tr>
          <tr>
            <th v-for="column in TABLE_COLUMNS"
              :key="column.key"
              :class="{ 'left-align': column.key === COLUMN_KEYS.NAME }"
              @click="sortBy(column.key)">
              {{ column.label }}
              <span v-if="sortColumn === column.key">
                {{ sortDirection === SORT_DIRECTIONS.ASC ? UI_ICONS.SORT_ASC : UI_ICONS.SORT_DESC }}
              </span>
            </th>
            <th class="th-action">{{ UI_LABELS.ACTIONS }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!path">
            <td colspan="5" class="empty-folder">
              {{ ERROR_MESSAGES.PATH_REQUIRED }}
            </td>
          </tr>
          <FileRow
            v-else-if="folders.length"
            v-for="(item, index) in sortedFiles"
            :key="index"
            :file="item"
            @open-child-folder="navigateToChildFolder"
          />
          <tr v-else>
            <td colspan="5" class="empty-folder">
              {{ UI_ICONS.EMPTY_FOLDER }} {{ UI_LABELS.EMPTY_FOLDER }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import '@/assets/css/file_browser.css'
import apiClient from '@/api'
import FileRow from './FileRow.vue'
import {
  COLUMN_KEYS,
  TABLE_COLUMNS,
  API_ENDPOINTS,
  DEFAULT_SORT,
  ERROR_MESSAGES,
  UI_LABELS,
  UI_ICONS,
  SORT_DIRECTIONS,
} from '@/constants/config'

export default {
  components: { FileRow },
  data() {
    return {
      folders: [],
      sortColumn: DEFAULT_SORT.COLUMN,
      sortDirection: DEFAULT_SORT.DIRECTION,
      currentPath: '',
      currentPathBreadCrumb: '',
      COLUMN_KEYS,
      TABLE_COLUMNS,
      UI_LABELS,
      UI_ICONS,
      SORT_DIRECTIONS,
      ERROR_MESSAGES
    }
  },
  props: {
    path: String,
  },
  watch: {
    path(newValue) {
      if (newValue) {
        this.currentPath = newValue
        this.fetchFiles(newValue)
      }
    },
  },
  methods: {
    async fetchFiles(path) {
      this.folders = [] // Reset on new path
      try {
        const response = await apiClient.get(
          `${API_ENDPOINTS.GET_DIRECTORIES}?path=${encodeURIComponent(path)}`
        )
        if (response.status === 200) {
          this.currentPathBreadCrumb = response.data.path
            .replace('/', '')
            .replace(/\//g, ` ${UI_LABELS.BREADCRUMB_SEPARATOR} `)
          this.folders = response.data.children || []
        } else {
          console.error(`HTTP error! Status: ${response.status}`)
        }
      } catch (err) {
        if (err.status === 400) {
          alert(ERROR_MESSAGES.GET_DIRECTORIES_INVALID_PATH)
        } else if (err.status === 404) {
          alert(ERROR_MESSAGES.GET_DIRECTORIES_NOT_FOUND)
        } else {
          console.error(err.message)
        }
      }
    },
    sortBy(column) {
      if (this.sortColumn === column) {
        this.sortDirection = this.sortDirection === SORT_DIRECTIONS.ASC ? SORT_DIRECTIONS.DESC : SORT_DIRECTIONS.ASC
      } else {
        this.sortColumn = column
        this.sortDirection = SORT_DIRECTIONS.ASC
      }
    },
    navigateToChildFolder(folderName) {
      this.currentPath = `${this.currentPath}/${folderName}`
      this.fetchFiles(this.currentPath)
    },
    navigateToParentFolder() {
      this.currentPath = this.currentPath.substring(0, this.currentPath.lastIndexOf('/'))
      this.fetchFiles(this.currentPath)
    },
  },
  computed: {
    sortedFiles() {
      return [...this.folders].sort((a, b) => {
        let result = 0
        if (a.type !== b.type) return a.type === 'directory' ? -1 : 1

        if (this.sortColumn === COLUMN_KEYS.NAME) {
          result = a.name.localeCompare(b.name)
        } else if (this.sortColumn === COLUMN_KEYS.SIZE) {
          result = (a.size || 0) - (b.size || 0)
        } else if (this.sortColumn === COLUMN_KEYS.CREATED) {
          result = new Date(a.creationDateTime || 0) - new Date(b.creationDateTime || 0)
        } else if (this.sortColumn === COLUMN_KEYS.MODIFIED) {
          result = new Date(a.modifiedDateTime || 0) - new Date(b.modifiedDateTime || 0)
        }

        return this.sortDirection === SORT_DIRECTIONS.ASC ? result : -result
      })
    },
  },
}
</script>
