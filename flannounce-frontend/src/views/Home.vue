<template>
  <div class="home">
    <!-- <img alt="Vue logo" src="../assets/logo.png" /> -->
    <AnnouncesList />
  </div>
</template>

<script>
// @ is an alias to /src
import AnnouncesList from '@/components/AnnouncesList.vue'

export default {
  name: 'Home',
  components: {
    AnnouncesList
  }
}
</script>
