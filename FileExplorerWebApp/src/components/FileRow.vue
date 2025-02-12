<template>
  <tr @click="toggleFolder" class="folder-row">
    <td :style="{ paddingLeft: depth * 20 + 'px' }">
      <span v-if="folder.type === 'directory'">
        <span class="icon">{{ expanded ? '📂' : '📁' }}</span>
        <strong>{{ folder.name }}</strong>
      </span>

      <span v-else>
        <span class="icon">📄</span>
        {{ folder.name }}
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

export default {
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

    // Reset when new path is entered
    watch(
      () => props.folder,
      () => {
        expanded.value = false
        subfolders.value = []
      },
    )

    return { expanded, subfolders, toggleFolder }
  },
}
</script>
