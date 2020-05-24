<template>
  <div className="flat">
    <h3>{{ announcement.title }}</h3>
    <a className="deleteButton" @click="handleDelete">delete</a>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { Announce } from '@/types/announce';

export default defineComponent({
  props: {
    announcement: {
      required: true,
      type: Object as PropType<Announce>
    }
  },
  setup() {
    const handleDelete = () => {
      alert('delete');
    };
    return { handleDelete };
  }
});
</script>

<style lang="scss" scoped>
.deleteButton {
  color: tomato;
  text-decoration: none;
  margin-left: 1rem;
  cursor: pointer;
}

.flat > h3 {
  display: inline-block;
}
</style>
