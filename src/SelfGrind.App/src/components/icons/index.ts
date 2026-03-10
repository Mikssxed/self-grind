import addUser from './AddUser.vue';
import analytics from './Analytics.vue';
import character from './Character.vue';
import community from './Community.vue';
import contributionGrid from './ContributionGrid.vue';
import dailyTasks from './DailyTasks.vue';
import gamepad from './GamePad.vue';
import house from './House.vue';
import lock from './Lock.vue';
import mail from './Mail.vue';
import menu from './Menu.vue';
import user from './User.vue';

const icons = {
    addUser,
    analytics,
    character,
    community,
    contributionGrid,
    dailyTasks,
    user,
    mail,
    lock,
    gamepad,
    house,
    menu,
};

export type IconName = keyof typeof icons;

export default icons;
