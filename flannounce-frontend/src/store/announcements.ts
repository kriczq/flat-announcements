import {
  getModule,
  VuexModule,
  Module,
  Mutation,
  Action
} from 'vuex-module-decorators';

import store from '@/store';
import { Announcement } from '@/types/announcement';
import announcementsApi from '@/api/announcements';

type Status = 'LOADING' | 'ERROR' | 'LOADED';

@Module({
  dynamic: true,
  namespaced: true,
  name: 'announcements',
  store
})
export default class AnnouncementsModule extends VuexModule {
  public announcements: Announcement[] = [];

  public status: Status = 'LOADING';

  @Action
  async fetchAnnouncements() {
    this.startLoading();

    try {
      const announcements = await announcementsApi.fetchAnnouncements();
      this.addAnnouncements(announcements);
      this.finishLoading();
    } catch {
      this.loadingError();
    }
  }

  @Mutation
  addAnnouncement(announcement: Announcement) {
    this.announcements.push(announcement);
  }

  @Mutation
  addAnnouncements(announcements: Announcement[]) {
    this.announcements.push(...announcements);
  }

  @Mutation
  startLoading() {
    this.status = 'LOADING';
  }

  @Mutation
  finishLoading() {
    this.status = 'LOADED';
  }

  @Mutation
  loadingError() {
    this.status = 'ERROR';
  }
}

export const announcementsModule = getModule(AnnouncementsModule, store);
