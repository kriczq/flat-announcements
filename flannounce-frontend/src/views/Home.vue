<template>
  <div class="home">
    <AnnouncementsList />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

import AnnouncementsList from '@/components/AnnouncementsList.vue';

export default defineComponent({
  name: 'Home',
  components: {
    AnnouncementsList
  },
  setup() {
    return {};
  }
});
</script>
