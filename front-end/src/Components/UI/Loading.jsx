import React from "react";

import Title from "./Title";

/**
 * A loading screen to show users when Async calls are being done.
 */
const Loading = () => {
  return (
    <React.Fragment>
      <Title Title="Loading..." isLoading={true} />
    </React.Fragment>
  );
};

export default Loading;
