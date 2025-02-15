<template>
  <tr :class="[file.type === FILE_TYPES.FILE ? 'file-tr' : '']" @click="openChildFolder">
    <td class="left-align">
      <span v-if="file.type === FILE_TYPES.DIRECTORY">{{ UI_ICONS.FOLDER }}</span>
      <span v-else>
        <FileIcons :name="file.name" :width="20" :height="20" :isFolder="false" :isMulti="false" :isLink="false"
          :itemStyle="{ display: 'inline', alignItems: 'center' }" />
      </span>
      <span class="file-name">
        {{ file.name }}
      </span>
    </td>
    <td>{{ formatSize(file.size) || UI_LABELS.DEFAULT_VALUE }}</td>
    <td>{{ formatDate(file.creationDateTime) || UI_LABELS.DEFAULT_VALUE }}</td>
    <td>{{ formatDate(file.modifiedDateTime) || UI_LABELS.DEFAULT_VALUE }}</td>
    <td>
      <span v-if="file.type === FILE_TYPES.FILE">
        <button @click.stop="copyPath(file.path)" class="copy-btn">{{ UI_ICONS.COPY }}</button>
      </span>
    </td>
  </tr>
</template>

<script>
import '@/assets/css/file_row.css'
import moment from 'moment'
import FileIcons from 'file-icons-vue'
import {
  UI_ICONS,
  UI_LABELS,
  FILE_TYPES
} from '@/constants/config'

export default {
  props: {
    file: Object,
  },
  components: {
    FileIcons,
  },
  data() {
    return {
      UI_ICONS,
      UI_LABELS,
      FILE_TYPES
    }
  },
  emits: ['open-child-folder'],
  methods: {
    formatDate(date) {
      return date ? moment(date).format(UI_LABELS.DATE_FORMAT) : UI_LABELS.DEFAULT_VALUE
    },
    formatSize(size) {
      if (!size) return UI_LABELS.DEFAULT_VALUE
      return size < 1024
        ? `${size} B`
        : size < 1048576
          ? `${(size / 1024).toFixed(1)} KB`
          : size < 1073741824
            ? `${(size / 1048576).toFixed(1)} MB`
            : `${(size / 1073741824).toFixed(1)} GB`
    },
    openChildFolder() {
      if (this.file.type === FILE_TYPES.DIRECTORY) {
        this.$emit('open-child-folder', this.file.name)
      }
    },
    copyPath(filePath) {
      navigator.clipboard
        .writeText(filePath)
        .then(() => {
          alert(`${UI_LABELS.COPY_SUCCESS} "${filePath}"`)
        })
        .catch((err) => {
          alert(`${UI_LABELS.COPY_FAIL}: ${err}`)
        })
    },
  },
}
</script>
