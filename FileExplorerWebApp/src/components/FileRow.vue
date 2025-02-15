<template>
  <tr>
    <td class="left-align">{{ file.name }}</td>
    <td>{{ formatSize(file.size) || '-' }}</td>
    <td>{{ formatDate(file.creationDateTime) || '-' }}</td>
    <td>{{ formatDate(file.modifiedDateTime) || '-' }}</td>
    <td></td>
  </tr>
</template>

<script>
import '@/assets/css/file_row.css'
import moment from 'moment'

export default {
  props: {
    file: Object,
  },
  methods: {
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
  },
}
</script>
