<template>
  <v-app>
    <v-app-bar app color="indigo accent-3" dark>
      <div class="d-flex align-center">
        <router-link
          class="text-h4 mr-5 white--text"
          style="text-decoration: none;"
          :to="{ name: 'home' }"
        >
          <v-icon class="mr-4" style="font-size: 36px;">mdi-home-city-outline</v-icon><span class="logo-title">Flannounce</span>
        </router-link>
        <v-btn :to="{ name: 'home' }" class="nav-btn mr-2" text exact
          ><v-icon class="mr-2">mdi-newspaper-variant-multiple</v-icon>Oferty mieszkań</v-btn
        >
        <v-btn :to="{ name: 'map' }" class="nav-btn mr-2" text exact><v-icon class="mr-2">mdi-google-maps</v-icon>Mapa</v-btn>
        <v-btn :to="{ name: 'stats' }" class="nav-btn" text exact
          ><v-icon class="mr-2">mdi-chart-bar</v-icon>Statystyki</v-btn
        >
      </div>
      <!-- <div class="d-flex align-center">
        <v-img
          alt="Vuetify Logo"
          class="shrink mr-2"
          contain
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-logo-dark.png"
          transition="scale-transition"
          width="40"
        />

        <v-img
          alt="Vuetify Name"
          class="shrink mt-1 hidden-sm-and-down"
          contain
          min-width="100"
          src="https://cdn.vuetifyjs.com/images/logos/vuetify-name-dark.png"
          width="100"
        />
      </div> -->

      <!-- <v-spacer></v-spacer>

      <v-btn
        href="https://github.com/vuetifyjs/vuetify/releases/latest"
        target="_blank"
        text
      >
        <span class="mr-2">Latest Release</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn> -->
    </v-app-bar>

    <v-main>
      <router-view :key="$route.path" class="content-wrap" />
    </v-main>
    <v-progress-linear
      v-if="loading"
      class="loading-bar"
      indeterminate
      color="deep-purple accent-4"
    />
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { appModule } from '@/store/app'

@Component({
  name: 'App'
})
export default class App extends Vue {
  private get loading() {
    return appModule.loading
  }
}
</script>

<style lang="scss" scoped>
.loading-bar {
  background-color: rgba(255, 255, 255, 0.5);
  position: fixed;
  top: 60px;
  z-index: 10;
}

.nav-btn.v-btn--active::before {
  opacity: 0;
}

::v-deep .nav-btn > .v-btn__content {
  padding: 4px 0;
}

::v-deep .nav-btn.v-btn--active > .v-btn__content {
  border-bottom: 2px solid white;
}

@media (max-width: 800px) {
  .logo-title {
    display: none;
  }
}
</style>
