<template>
  <p v-if="status === 'LOADING'">loading...</p>
  <p v-else-if="status === 'ERROR'" style="color: tomato">loading error</p>
  <p v-else-if="announcements.length === 0">
    there is no announcements actually, check again later
  </p>
  <div v-else>
    <Announcement v-for="a in announcements" :key="a.title" :announcement="a" />
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue';
import Announcement from '@/components/Announcement.vue';
import { announcementsModule } from '@/store/announcements';

export default defineComponent({
  components: {
    Announcement
  },
  setup() {
    const { announcements, fetchAnnouncements } = announcementsModule;
    fetchAnnouncements();

    return {
      announcements,
      status: computed(() => announcementsModule.status)
    };
  }
});
</script>
