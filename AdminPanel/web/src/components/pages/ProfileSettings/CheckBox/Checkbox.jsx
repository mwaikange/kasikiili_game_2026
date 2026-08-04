import React, { useState } from "react";
import styles from "./CheckBox.module.css";

const CheckBox = ({ label, checked, setChecked }) => {
  return (
    <label className={styles.container}>
      <input
        type="checkbox"
        value={checked}
        checked={checked}
        onChange={() => setChecked((prev) => !prev)}
        className={styles.myCheckBox}
      />
      {/* <span className={styles.checkmark}></span>  */}
      {label}
    </label>
  );
};

export default CheckBox;
