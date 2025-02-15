export const TABLE_COLUMNS = [
  { key: 'name', label: 'Name' },
  { key: 'size', label: 'Size' },
  { key: 'created', label: 'Created' },
  { key: 'modified', label: 'Modified' },
]

export const API_ENDPOINTS = {
  GET_DIRECTORIES: '/api/DirectoryPath/directories',
}

export const DEFAULT_SORT = {
  COLUMN: 'name',
  DIRECTION: 'asc',
}

export const SORT_DIRECTIONS = {
  ASC: 'asc',
  DESC: 'desc',
}

export const COLUMN_KEYS = {
  NAME: 'name',
  SIZE: 'size',
  CREATED: 'created',
  MODIFIED: 'modified',
}

export const FILE_TYPES = {
  FILE: 'file',
  DIRECTORY: 'directory',
}

export const ERROR_MESSAGES = {
  PATH_REQUIRED: 'Path is required.',
  GET_DIRECTORIES_INVALID_PATH: 'Please enter a valid folder path.',
  GET_DIRECTORIES_NOT_FOUND: 'The specified folder path was not found.',
  INPUT_PATH_REQUIRED: 'Please enter a folder path.',
}

export const UI_LABELS = {
  APP_TITLE: '📁 File Browser',
  INPUT_PLACEHOLDER: 'Enter a path (e.g., /home/user)',
  BROWSE_BUTTON: 'Browse',
  BACK_BUTTON: 'Back',
  ACTIONS: 'Actions',
  EMPTY_FOLDER: 'Empty folder',
  BREADCRUMB_SEPARATOR: '>',
  DEFAULT_VALUE: '-',
  DATE_FORMAT: 'DD/MM/YYYY hh:mm:ss A',
  COPY_SUCCESS: 'Path has been copied to the clipboard!',
  COPY_FAIL: 'Failed to copy path',
}

export const UI_ICONS = {
  BACK: '◀︎',
  SORT_ASC: '▲',
  SORT_DESC: '▼',
  EMPTY_FOLDER: '📂',
  FOLDER: '📁',
  COPY: '📋 Copy Path',
}
