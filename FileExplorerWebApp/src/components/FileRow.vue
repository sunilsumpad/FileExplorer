<template>
  <tr @click="handleClick" :class="{ folder: file.type === 'directory' }">
    <td class="left-align">
      <span v-if="file.type === 'directory'">📁 {{ file.name }}</span>
      <span v-else>
        <FileIcons
          :name="file.name"
          :width="20"
          :height="20"
          :isFolder="false"
          :isMulti="false"
          :isLink="false"
          :itemStyle="{ display: 'inline', alignItems: 'center' }"
        />
        <span>{{ file.name }}</span>
      </span>
    </td>
    <td>{{ formatSize(file.size) || '-' }}</td>
    <td>{{ formatDate(file.creationDateTime) }}</td>
    <td>{{ formatDate(file.modifiedDateTime) }}</td>
    <td>
      <span v-if="file.type === 'file'"
        ><button @click.stop="copyPath(file.path)" class="copy-btn">📋 Copy Path</button>
      </span>
    </td>
  </tr>
</template>

<script>
import '@/assets/css/file_browser.css'
import FileIcons from 'file-icons-vue'
import moment from 'moment'

export default {
  props: {
    file: Object,
  },
  components: {
    FileIcons,
  },
  emits: ['open-folder'],
  methods: {
    handleClick() {
      if (this.file.type === 'directory') {
        this.$emit('open-folder', this.file.name)
      }
    },
    formatDate(date) {
      return date ? moment(date).format('DD/MM/YYYY hh:mm:ss A') : '-'
    },
    formatSize(size) {
      if (!size) return '--'
      return size < 1024
        ? `${size} B`
        : size < 1048576
          ? `${(size / 1024).toFixed(1)} KB`
          : size < 1073741824
            ? `${(size / 1048576).toFixed(1)} MB`
            : `${(size / 1073741824).toFixed(1)} GB`
    },
    copyPath(path) {
      navigator.clipboard
        .writeText(path)
        .then(() => {
          alert('Path: "' + path + '" has been copied to the clipboard!')
        })
        .catch((err) => {
          console.error('Failed to copy path: ', err)
        })
    },
  },
}
</script>
