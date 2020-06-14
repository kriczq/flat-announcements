import { Component, Vue } from 'vue-property-decorator'

@Component
export class ScrollMixin extends Vue {
  private scrollHandler() {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 1) {
      this.scrolled()
    }
  }

  private mounted() {
    window.addEventListener('scroll', this.scrollHandler)
  }

  private destroyed() {
    window.removeEventListener('scroll', this.scrollHandler)
  }

  // eslint-disable-next-line
  protected scrolled() {}
}
