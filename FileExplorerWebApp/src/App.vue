<template>
  <div class="app">
    <header class="header">{{ UI_LABELS.APP_TITLE }}</header>

    <div class="input-container">
      <input v-model="inputPath" name="inputPath" :placeholder="UI_LABELS.INPUT_PLACEHOLDER" />
      <button class="default-btn-class" @click="sendPath">{{ UI_LABELS.BROWSE_BUTTON }}</button>
    </div>
    <FileBrowser :path="sentPath"/>
  </div>
</template>

<script>
import '@/assets/css/app.css'
import FileBrowser from './components/FileBrowser.vue'
import { UI_LABELS, ERROR_MESSAGES } from '@/constants/config'

export default {
  components: { FileBrowser },
  data() {
    return {
      inputPath: '',
      sentPath: '',
      UI_LABELS
    }
  },
  methods: {
    sendPath() {
      if (this.inputPath) {
        this.sentPath = this.inputPath
      } else {
        alert(ERROR_MESSAGES.INPUT_PATH_REQUIRED)
      }
    }
  }
}
</script>
