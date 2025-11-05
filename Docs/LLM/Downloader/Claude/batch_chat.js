const sleep = (timeout) => {
  return new Promise((resolve) => {
    setTimeout(resolve, timeout);
  });
};
const data = [];
document.addEventListener("click", () => {
  alert();
  navigator.clipboard.writeText(JSON.stringify(data, null, 2));
});
const getData = async (uuids) => {
  for (var i = 0; i < uuids.length; i++) {
    const uuid = uuids[i];
    const response = await fetch(
      `https://claude.ai/api/organizations/fab05e05-db4c-4d79-8fa0-640cd2946b43/chat_conversations/${uuid}?tree=True&rendering_mode=messages&render_all_tools=true`
    );
    const result = await response.json();
    console.log(result);
    data.push(result);
    await sleep(2000);
  }
  alert("done");
};

const getUUIDs = async () => {
  const projectId = window.location.pathname.split("/").pop();
  const response = await fetch(
    `https://claude.ai/api/organizations/fab05e05-db4c-4d79-8fa0-640cd2946b43/projects/${projectId}/conversations`
  );
  const result = await response.json();
  return result.map((r) => r.uuid);
};

const start = async () => {
  const uuids = await getUUIDs();
  console.log(uuids);
  await getData(uuids);
}
start();