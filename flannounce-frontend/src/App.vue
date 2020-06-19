<template>
  <v-app>
    <v-app-bar app color="indigo accent-3" dark>
      <div class="d-flex align-center">
        <router-link
          class="text-h4 mr-10 white--text"
          style="opacity: 0.7; text-decoration: none;"
          :to="{ name: 'home' }"
        >
          Flannounce
        </router-link>
        <v-btn :to="{ name: 'home' }" class="nav-btn mr-3" text exact
          >Oferty mieszkań</v-btn
        >
        <v-btn :to="{ name: 'stats' }" class="nav-btn" text exact
          >Statystyki</v-btn
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

    <v-content>
      <router-view :key="$route.path" class="content-wrap" />
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({
  name: 'App'
})
export default class App extends Vue {}
</script>

<style lang="scss" scoped>
.nav-btn.v-btn--active::before {
  opacity: 0;
}

::v-deep .nav-btn > .v-btn__content {
  padding: 4px;
}

::v-deep .nav-btn.v-btn--active > .v-btn__content {
  border-bottom: 2px solid white;
}
</style>
