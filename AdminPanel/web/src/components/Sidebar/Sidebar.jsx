import React from "react";
import { NavLink } from "react-router-dom";
import {
  user,
  distributor,
  home,
  setting,
  appmangement,
} from "../../assets/image/images";
import styles from "./styles.module.css";

const Sidebar = ({navdata}) => {

  // const navItems = [
  //   {
  //     icon: distributor,
  //     navItem: "DISTRIBUTORS",
  //     to: "/",
  //   },
  //   {
  //     icon: user,
  //     navItem: "USERS",
  //     to: "/user",
  //   },
  //   {
  //     icon: appmangement,
  //     navItem: "APP MANAGEMENT",
  //     to: "/appmanagement",
  //   },
  // ];

  return (
    <section className={styles.sidebar}>
      <div className={styles.topItem}>
        <div className={`${styles.navItems} `}>
          <img src={home} alt="#" className={styles.icon} />
          <p className={`${styles.navItem} ${styles.home}`}>Home</p>
        </div>
        {navdata.length > 0 ? navdata.map((el, i) => (
          <div key={i}>
            <NavLink
              to={el.to}
              className={({ isActive }) =>
                isActive
                  ? `${styles.activeNavItems} ${styles.navItems}`
                  : styles.navItems
              }
            >
              {" "}
              <img src={el.icon === 'distributor' ? distributor : el.icon === 'user' ? user : appmangement} alt="#" className={styles.icon} />
              {el.navItem}
            </NavLink>
          </div>
        )) : ''}
      </div>
      <div>
        <NavLink
          to="/profileSettings"
          className={({ isActive }) =>
            isActive
              ? `${styles.activeNavItems} ${styles.navItems}`
              : styles.navItems
          }
        >
          {" "}
          <img src={setting} alt="#" className={styles.icon} />
          PROFILE SETTINGS
        </NavLink>

        <p className={styles.version}> VERSION 1.0000</p>
      </div>
    </section>
  );
};

export default Sidebar;
