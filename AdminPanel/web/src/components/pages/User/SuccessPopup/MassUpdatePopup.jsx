import React from "react";
import styles from "./styles.module.css";

const MassSuccess = ({ setModal }) => {
  return (
    <div>
      <div className={styles.popup}>
        <p className={styles.message}>5285 cashout records updated !!</p>{" "}
      </div>{" "}
      <div className={styles.overlay} onClick={() => setModal(false)}></div>
    </div>
  );
};

export default MassSuccess;
