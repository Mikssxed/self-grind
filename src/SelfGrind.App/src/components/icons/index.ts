import addUser from './AddUser.vue';
import user from './User.vue';

const icons = {
    addUser,
    user,
};

export type IconName = keyof typeof icons;

export default icons;
