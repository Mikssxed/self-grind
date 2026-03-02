import addUser from './AddUser.vue';
import gamepad from './GamePad.vue';
import house from './House.vue';
import lock from './Lock.vue';
import mail from './Mail.vue';
import user from './User.vue';

const icons = {
    addUser,
    user,
    mail,
    lock,
    gamepad,
    house,
};

export type IconName = keyof typeof icons;

export default icons;
