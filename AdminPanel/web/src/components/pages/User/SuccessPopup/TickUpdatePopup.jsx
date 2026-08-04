import React from "react";
import styles from "./styles.module.css";

const TickUpdate = ({ setModal }) => {
  return (
    <div>
      <div className={styles.popup}>
        <p className={styles.message}>35 cashout records updated !!</p>{" "}
      </div>{" "}
      <div className={styles.overlay} onClick={() => setModal(false)}></div>
    </div>
  );
};

export default TickUpdate;
