<template>
  <tr @click="toggleFolder" class="folder-row">
    <td :style="{ paddingLeft: depth * 20 + 'px' }">
      <!-- FOLDER -->
      <span v-if="folder.type === 'directory'">
        <span class="icon">{{ expanded ? '📂' : '📁' }}</span>
        <strong>{{ folder.name }}</strong>
      </span>
      <!-- FILE -->
      <span v-else>
        <FileIcons :name="folder.name" :width="20" :height="20" :isFolder="false" :isMulti="false" :isLink="false"
          :itemStyle="{ display: 'inline', alignItems: 'center' }" />
        <span>{{ folder.name }}</span>
        <button @click.stop="copyPath(folder.path)" class="copy-btn">📋 Copy Path</button>
      </span>
    </td>
  </tr>

  <transition-group name=" slide">
    <div style="display: contents" v-if="expanded && folder.type === 'directory'">
      <file-row v-for="(subfolder, index) in subfolders" :key="index" :folder="subfolder" :depth="depth + 1" />
    </div>
  </transition-group>
</template>

<script>
import { ref, watch } from 'vue'
import '@/assets/css/filerow.css'
import apiClient from '@/api'
import FileIcons from 'file-icons-vue';

export default {
  components: {
    FileIcons
  },
  props: {
    folder: Object,
    depth: { type: Number, default: 1 },
  },
  setup(props) {
    const expanded = ref(false)
    const subfolders = ref([])

    const toggleFolder = async () => {
      if (props.folder.type !== 'directory') return

      if (expanded.value) {
        expanded.value = false
      } else {
        try {
          const response = await apiClient.get(
            `/api/DirectoryPath/directories?path=${encodeURIComponent(props.folder.path)}`,
          )
          if (response.status >= 200 && response.status < 300) {
            const data = await response.data
            subfolders.value = data.children || []
          } else {
            throw new Error(`HTTP error! Status: ${response.status}`)
          }
        } catch (err) {
          console.error(err.message)
        }
        expanded.value = true
      }
    }
    const copyPath = (path) => {
      navigator.clipboard.writeText(path)
        .then(() => {
          alert('Path: ' + path + ' copied to clipboard!')
        })
        .catch((err) => {
          console.error('Failed to copy path: ', err)
        })
    }
    // Reset when new path is entered
    watch(
      () => props.folder,
      () => {
        expanded.value = false
        subfolders.value = []
      },
    )

    return { expanded, subfolders, toggleFolder, copyPath }
  },
}
</script>
