import React from "react";
import CreditDistributor from "./CreditDistributor";
import CreditGeneration from "./CreditGeneration";
import DistributorBlock from "./DistributorBlock";
import styles from "./styles.module.css";
import UserBlock from "./UserBlock";

const AppMangeMent = () => {
  const headingData = [
    { key: "AGENT CODE:", value: "XP48585" },
    { key: "AGENT NAME:", value: "XLLEWELLYN MOTINGA" },
    { key: "AGENT BRANCH:", value: "KEETMANSHOOP" },
  ];
  return (
    <div>
      <div className={styles.header}>
        {headingData.map((el, i) => (
          <p className={styles.keyValue} key={i}>
            <span>{el.key}</span>
            <span className={styles.value}>{el.value}</span>
          </p>
        ))}
      </div>
      <div className={styles.wrapper}>
        <CreditGeneration />
        <CreditDistributor />
        <DistributorBlock />
        <UserBlock />
      </div>
    </div>
  );
};

export default AppMangeMent;
