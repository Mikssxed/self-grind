import addUser from './AddUser.vue';
import lock from './Lock.vue';
import mail from './Mail.vue';
import user from './User.vue';

const icons = {
    addUser,
    user,
    mail,
    lock,
};

export type IconName = keyof typeof icons;

export default icons;
